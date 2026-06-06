# Contributing to DynDnsProxy

Thanks for your interest in improving DynDnsProxy! Contributions of all kinds
are welcome — bug reports, fixes, docs, and features.

## Getting started

```bash
# Build
dotnet build

# Run the tests
dotnet test

# Run the app locally
dotnet run --project DynDnsProxy/DynDnsProxy.csproj
```

The project targets **.NET 10** (ASP.NET Core). See [`README.md`](README.md) for
configuration and the request flow.

## Making changes

1. Fork the repo and create a branch off `main`.
2. Keep changes focused; one logical change per pull request.
3. Add or update tests under `Test/Test.DynDnsProxy/` for any behaviour change.
4. Make sure `dotnet build` and `dotnet test` pass before opening the PR.
5. Match the existing code style (nullable enabled, implicit usings).

## Changelog

We keep a [Keep a Changelog](https://keepachangelog.com)-style
[`CHANGELOG.md`](CHANGELOG.md). For any user-facing change, add a bullet under
the `## [Unreleased]` section in the same PR, grouped under `Added`, `Changed`,
`Fixed`, etc. Don't add a version heading or date — maintainers rename
`[Unreleased]` to the new version at release time.

## Pull requests

- Describe **what** changed and **why** in the PR description.
- Link any related issue.
- CI (build, tests, CodeQL, dependency review) must pass.

## Reporting bugs / requesting features

Use the [issue templates](https://github.com/DotPhiB/DynDnsProxy/issues/new/choose).
For security issues, follow [`SECURITY.md`](SECURITY.md) instead of opening a
public issue.
