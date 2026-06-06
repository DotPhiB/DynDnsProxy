# Security Policy

## Supported Versions

DynDnsProxy is released as a rolling image from `main`. Security fixes land in
the **newest release only** — there is no backporting to older versions. To stay
patched, run the most recent `ghcr.io/dotphib/dyndnsproxy` image.

Each release publishes several tags: a fixed patch tag (e.g. `1.1.1`), the
rolling `1.1` and `1` tags, and `latest`. Pin to a patch tag like `1.1.1` for a
reproducible, immutable image — but note that staying on an old patch means you
will **not** receive fixes. The `1`, `1.1`, and `latest` tags advance to the
newest release, so following one of those is how you keep getting security
updates.

## Reporting a Vulnerability

Please **do not** open a public issue for security vulnerabilities.

Instead, report privately via GitHub's
[private vulnerability reporting](https://github.com/DotPhiB/DynDnsProxy/security/advisories/new).

Please include:

- a description of the issue and its impact,
- steps to reproduce (a proof of concept if possible),
- affected version / image tag.

You can expect an initial acknowledgement within a few days. Once the issue is
confirmed and fixed, a new image will be published and the advisory disclosed.
