dotnet build ./src/LedgerLite.WebApi || exit 1 
dotnet ef database update --startup-project ./src/LedgerLite.WebApi --project ./src/LedgerLite.Accounting.Core --context AccountingDbContext --no-build
dotnet ef database update --startup-project ./src/LedgerLite.WebApi --project ./src/LedgerLite.Users --context UsersDbContext --no-build