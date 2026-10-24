migrate:
	dotnet run --project BookStore.Cli -- migrate

seed:
	dotnet run --project BookStore.Cli -- seed

fresh:
	dotnet run --project BookStore.Cli -- fresh --seed