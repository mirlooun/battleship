Update dotnet-ef
~~~
dotnet tool update --global dotnet-ef
~~~

Database useful commands
~~~
dotnet ef migrations add InitialCreate --project DAL --startup-project Application

dotnet ef database update --project DAL --startup-project Application

dotnet ef database drop --project DAL --startup-project Application
~~~

Razor Pages scaffold
~~~
dotnet aspnet-codegenerator razorpage -m MODEL -dc AppDbContext -udl -outDir Pages/MODELS --referenceScriptLibraries -f
~~~
