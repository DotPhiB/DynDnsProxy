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

Run DynDnsProxy on a local docker host (e.g. 192.168.178.2):

``` yml
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

Configure your fritzbox to call dyndns with the following UpdateUrl, but replace the ip6 with your servers ip6 first:
```
http://192.168.178.2:8080/DynDns/Update?domain=<domain>&ip4=<ipaddr>&ip6lanprefix=<ip6lanprefix>&ip6=::1:1111:2222:3333:4444
```
> Caution: your server ip6 may need to be prefixed with `::1:`, if your fritzbox provides ip6 addresses in the first local subnet by default. Check the servers IPv6-GUA to be sure about that.

The fritzbox will now call the local DynDnsProxy, whenever your ip4 or ip6 changes.

## Security considerations

If you run this image locally and call it with explicit docker-host ip, there should be no issue.

If you want to run your proxy with authentication, you can use a reverse-proxy (e.g. nginx) to add that.
