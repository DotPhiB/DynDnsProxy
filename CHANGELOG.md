# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.1.1] - 2026-07-25

### Security

- Fixed CVE-2026-49451 / GHSA-v5pm-xwqc-g5wc: the transitive `Microsoft.OpenApi`
  dependency (2.0.0, pulled in via `Microsoft.AspNetCore.OpenApi`) could terminate
  the process on parsing a circular schema reference. Adopted NuGet Central Package
  Management with transitive pinning and pinned `Microsoft.OpenApi` to 2.11.0.

### Changed

- Adopted Central Package Management (`Directory.Packages.props`) for all package
  versions.
- `latest` now tracks the main line only: a patch tag cut from a maintenance branch
  refreshes its `1`/`1.x` tags without moving `latest` backwards.
- Configured Dependabot to keep `Microsoft.OpenApi` on the 2.x line until the
  framework's own floor moves.

## [1.1.0] - 2026-06-07

### Added

- Configuration is validated at startup (`ValidateOnStart`), so misconfiguration
  fails fast instead of at first request.
- Community health files, badges, and a rewritten README.
- Release automation: tagging `v*.*.*` publishes a GitHub Release and a signed
  container image to `ghcr.io/dotphib/dyndnsproxy`.

### Changed

- Upgraded target framework from .NET 8 to .NET 10.
- Replaced Swagger UI (Swashbuckle) with Scalar, served via the built-in
  `Microsoft.AspNetCore.OpenApi` document generator.
- Hardened CI workflows and pinned GitHub Action references to commit SHAs.
- Clarified the image tagging and security support policy: the `latest`, `1`,
  and `1.1` tags advance to the newest release, while a patch tag such as
  `1.1.1` pins a reproducible, immutable image.

### Fixed

- Corrected the outbound `User-Agent` header.
- Upstream provider status codes are now forwarded verbatim to the caller.
- Query inputs are URL-encoded before being substituted into the update URL.
- Hardened the IPv6 LAN-prefix combine logic.

## [1.0.0]

- Initial release: DynDNS update proxy with a single
  `GET /dyndns/update` endpoint.

[Unreleased]: https://github.com/DotPhiB/DynDnsProxy/compare/v1.1.1...HEAD
[1.1.1]: https://github.com/DotPhiB/DynDnsProxy/compare/v1.1.0...v1.1.1
[1.1.0]: https://github.com/DotPhiB/DynDnsProxy/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/DotPhiB/DynDnsProxy/releases/tag/v1.0.0
