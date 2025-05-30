docker compose up -d --build || exit 1
dotnet tool install dotnet-ef --create-manifest-if-needed || exit 1

dotnet build ./src/LedgerLite.WebApi || exit 1 
dotnet ef database update --startup-project ./src/LedgerLite.WebApi --project ./src/LedgerLite.Accounting.Core --context AccountingDbContext --no-build || exit 1
dotnet ef database update --startup-project ./src/LedgerLite.WebApi --project ./src/LedgerLite.Users --context UsersDbContext --no-build