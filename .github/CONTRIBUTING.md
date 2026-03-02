# Contributing to Novus

> **This is a proprietary project owned by Tojoo.**
> Contributions are restricted to authorized team members only.

## Branch Strategy

| Branch | Purpose |
|--------|---------|
| `main` | Production-ready code. Never commit directly. |
| `develop` | Integration branch for ongoing work. |
| `feat/*` | New features — branch off `develop`. |
| `fix/*` | Bug fixes — branch off `develop`. |
| `hotfix/*` | Urgent production fixes — branch off `main`. |

## Workflow

1. **Create a branch** from `develop` (or `main` for hotfixes).
2. **Write clean, focused commits** — one logical change per commit.
3. **Ensure quality** before pushing:
   - Code compiles without errors or warnings.
   - All existing tests pass (`dotnet test`).
   - New features include appropriate tests.
4. **Open a Pull Request** targeting `develop` (or `main` for hotfixes).
5. **Request review** from at least one team member.
6. **Address feedback**, then merge via squash or rebase.

## Commit Messages

Use clear, descriptive messages following this format:

```
<type>: <short summary>

<optional body — explain *why*, not *what*>
```

**Types:** `feat`, `fix`, `refactor`, `test`, `docs`, `chore`, `style`, `perf`

**Examples:**
- `feat: add API key rotation endpoint`
- `fix: resolve null reference in auth filter`
- `refactor: extract shared validation logic`

## Code Standards

- Follow existing project conventions and architecture.
- Use meaningful names for variables, methods, and classes.
- Keep methods short and single-purpose.
- Do not commit secrets, credentials, or environment-specific config.
- Remove dead code and unused imports before submitting.

## Questions?

Reach out to the project lead before starting work on significant changes.
