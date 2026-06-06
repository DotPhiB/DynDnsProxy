# DynDnsProxy

[![GitHub release](https://img.shields.io/github/v/release/DotPhiB/DynDnsProxy)](https://github.com/DotPhiB/DynDnsProxy/releases/latest)
[![.NET Tests](https://github.com/DotPhiB/DynDnsProxy/actions/workflows/execute-tests.yml/badge.svg)](https://github.com/DotPhiB/DynDnsProxy/actions/workflows/execute-tests.yml)
[![Docker Publish](https://github.com/DotPhiB/DynDnsProxy/actions/workflows/docker-publish.yml/badge.svg)](https://github.com/DotPhiB/DynDnsProxy/actions/workflows/docker-publish.yml)
[![CodeQL](https://github.com/DotPhiB/DynDnsProxy/actions/workflows/codeql.yml/badge.svg)](https://github.com/DotPhiB/DynDnsProxy/actions/workflows/codeql.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

A tiny proxy that fixes IPv6 handling for DynDNS updates.

Image: [`ghcr.io/dotphib/dyndnsproxy`](https://github.com/DotPhiB/DynDnsProxy/pkgs/container/dyndnsproxy)

## Why

Routers like the [FRITZ!Box](https://en.avm.de/service/knowledge-base/dial-help/) can send their `ip6lanprefix` but not the full IPv6 address of the host you actually want to register. DynDNS providers, however, expect the **combined** address (prefix + host part). Since the prefix rotates on every reconnect, the router can't build that address itself.

`DynDnsProxy` runs locally, combines the rotating prefix with your host's fixed suffix, forwards the update to your provider, and returns the provider's response verbatim.

## How it works

The proxy exposes one endpoint:

```
GET /dyndns/update?domain=<domain>&ip4=<ip4>&ip6=<ip6>&ip6LanPrefix=<prefix>
```

| Parameter | Required | Description |
|---|---|---|
| `domain` | yes | hostname to update |
| `ip4` | yes | IPv4 address |
| `ip6` | yes | IPv6 host suffix (combined with the prefix) |
| `ip6LanPrefix` | no | rotating LAN prefix, e.g. `2001:db8::/64` |

"Required" means the parameter must be present; empty values are accepted and passed through unchanged.

- `ip6LanPrefix` is combined with `ip6` into the full address. Omit it only for raw pass-through — without a prefix the proxy adds nothing over calling the provider directly.
- The full address plus `domain` and `ip4` are substituted into the configured `UpdateUrl` (placeholders `<domain>`, `<ip4>`, `<ip6>`), which is then called with HTTP Basic Auth.
- The provider's status code and body are passed straight back to the caller.

Inputs are URL-encoded; the provider's response is forwarded as-is (including error statuses).

## Configuration

All values are required and validated at startup. They bind from the `DynDns` config section via [`DynDns__` environment variables](https://learn.microsoft.com/aspnet/core/fundamentals/configuration/#environment-variables):

| Variable | Description |
|---|---|
| `DynDns__UpdateUrl` | Provider update URL with `<domain>`, `<ip4>`, `<ip6>` placeholders |
| `DynDns__UserName` | Basic Auth user for the provider |
| `DynDns__Password` | Basic Auth password for the provider |

## Example (FRITZ!Box → Strato)

Run the proxy on a local docker host (e.g. `192.168.178.2`):

```yml
services:
  my-proxy:
    image: ghcr.io/dotphib/dyndnsproxy:latest
    ports:
      - 8080:8080
    environment:
      DynDns__UpdateUrl: https://dyndns.strato.com/nic/update?hostname=<domain>&myip=<ip4>,<ip6>
      DynDns__UserName: ${strato-dyndns-user}
      DynDns__Password: ${strato-dyndns-password}
```

Point the FRITZ!Box DynDNS update URL at the proxy, hard-coding your server's IPv6 suffix:

```
http://192.168.178.2:8080/dyndns/update?domain=<domain>&ip4=<ipaddr>&ip6lanprefix=<ip6lanprefix>&ip6=::1:1111:2222:3333:4444
```

> The suffix may need a `::1:` prefix if your FRITZ!Box places hosts in the first local subnet. Verify against the server's IPv6 GUA.

The FRITZ!Box then calls the proxy whenever the IPv4 or IPv6 changes.

## Security

The proxy itself is unauthenticated. Run it locally and address it by explicit docker-host IP. To expose it with authentication, put a reverse proxy (e.g. nginx) in front.

## Enabling HTTPS

By default the container serves plain HTTP on `8080`. To enable HTTPS, mount a PKCS#12 (`.pfx`) certificate and configure [Kestrel](https://learn.microsoft.com/aspnet/core/fundamentals/servers/kestrel/endpoints#configure-https-in-appsettingsjson) via environment variables:

```yml
services:
  my-proxy:
    image: ghcr.io/dotphib/dyndnsproxy:latest
    ports:
      - 8081:8081
    volumes:
      - ./certs:/certs:ro
    environment:
      ASPNETCORE_HTTPS_PORTS: 8081
      ASPNETCORE_Kestrel__Certificates__Default__Path: /certs/cert.pfx
      ASPNETCORE_Kestrel__Certificates__Default__Password: ${cert-password}
      DynDns__UpdateUrl: https://dyndns.strato.com/nic/update?hostname=<domain>&myip=<ip4>,<ip6>
      DynDns__UserName: ${strato-dyndns-user}
      DynDns__Password: ${strato-dyndns-password}
```

Callers then use `https://...:8081/dyndns/update?...`.
