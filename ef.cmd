@echo off
dotnet ef %* -s JitEvolution.Api -p JitEvolution.Data -c JitEvolutionDbContext