# DynDnsProxy

Simple DynDns-Proxy to enable ip6 handling.

## The problem

- the local side allows to send `ip6lanprefix` but not the combined ip6 of your target server.
- the dyndns-provider expects the `ip6`, already prefixed with your `ip6lanprefix`.
- your `ip6lanprefix` is not stable and changes whenever you reconnect to your provider, so you cannot provide the actual ip6 without further handling

## The solution

Run `DynDnsProxy` locally, configure it with your provider url and authentication, and let your local side call it instead of the dyndns-provider.
It resolves the actual ip6, calls the dyndns-provider and returns the result.

## Example (fritzbox --> strato)

Run DynDnsProxy on a local docker host, which is reachable by your fritzbox with the following environment variables:
- DynDns__UpdateUrl: `https://dyndns.strato.com/nic/update?hostname=<domain>&myip=<ip4>,<ip6>`
- DynDns__UserName: {strato-dyndns-user}
- DynDns__Password: {strato-dyndns-password}

Configure your local fritzbox to call dyndns with the following UpdateUrl:
```
http://{docker-host}:{container-port}/DynDns/Update?domain=<domain>&ip4=<ipaddr>&ip6lanprefix=<ip6lanprefix>&ip6={target-server-ip6}
```
> Replace the values in curly brackets.
> - {docker-host} -> the domain or ip of your local docker-host
> - {container-port} -> the port of the DynDnsProxy container image
> - {target-server-ip6} -> the local ip6 of your server, which should be the target of DynDns

The fritzbox will now call the local DynDnsProxy, whenever there is a change in ips.

## Security considerations

If you run this image locally and call it with explicit docker-host ip, there should be no issue.

If you want to have an external proxy or need https and/or authentication for some reason, you can use a reverse-proxy (e.g. nginx) to add that.
