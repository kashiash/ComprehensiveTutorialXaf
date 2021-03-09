# Obsługa błędów w serwisie

## rejestrujemy kontroler do obsługi błedow

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if(env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    
    app.UseStatusCodePagesWithReExecute("/error/{0}");
    app.UseExceptionHandler("/error/500");
    ...
```


## kod kontrolera

```csharp
[ApiController]
public class ErrorController : ControllerBase
{
    [Route("/error-local-development")]
    public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
    {
        if(webHostEnvironment.EnvironmentName != "Development")
        {
            throw new InvalidOperationException("This shouldn't be invoked in non-development environments.");
        }

        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

        return Problem(detail: context.Error.StackTrace, title: context.Error.Message);
    }

    [Route("/error")]
    public IActionResult Error() => Problem();

    [Route("error/{code}")]
    public IActionResult Error(int code) { return new ObjectResult(new ApiResponse(code)); }
}
```



i klasa do zwracania wyniku w ładnej formie 

```csharp
public class ApiResponse
  {
      [JsonProperty("meta")]
      public Meta Meta { get; set; }

      public ApiResponse(int statusCode, string message = null)
      {
          Meta = new Meta();
          Meta.Status = StatusEnum.FAILED;
          Meta.Messages = new List<Messages>();
          Meta.Messages.Add(GetDefaultMessageForStatusCode(statusCode));
      }

      public ApiResponse(StatusEnum status, string code, LevelEnum level, string message)
      {
          Meta = new Meta();
          Meta.Status = status;
          Meta.Messages = new List<Messages>();
          Meta.Messages.Add(new Messages() { Code = code, Level = level, Text = message, });
      }


      private static Messages GetDefaultMessageForStatusCode(int statusCode)
      {
          switch(statusCode)
          {
              case 400:
                  return new Messages()
                  {
                      Code = "BAD_REQUEST",
                      Level = LevelEnum.ERROR,
                      Text = "Unable to process your request, please check its validity."
                  };
              case 401:
                  return new Messages()
                  {
                      Code = "UNAUTHORIZED",
                      Level = LevelEnum.ERROR,
                      Text =
                      "Missing, invalid or expired login and password. To fix, you should re-authenticate the user"
                  };
              case 403:
                  return new Messages()
                  { Code = "FORBIDDEN", Level = LevelEnum.ERROR, Text = "No access to action." };
              case 404:
                  return new Messages()
                  { Code = "NOT_FOUND", Level = LevelEnum.ERROR, Text = "The requested resource is not found." };
              case 500:
                  return new Messages()
                  {
                      Code = "INTERNAL_ERROR",
                      Level = LevelEnum.ERROR,
                      Text =
                      "Ożesz k..mać... Something wrong on the server! Please contact the administrator to report the issue."
                  };
              case 501:
                  return new Messages()
                  {
                      Code = "NOT_IMPLEMENTED",
                      Level = LevelEnum.ERROR,
                      Text =
                      "The server either does not recognize the request method, or it lacks the ability to fulfill the request."
                  };
              case 502:
                  return new Messages()
                  {
                      Code = "BAD_GATEWAY",
                      Level = LevelEnum.ERROR,
                      Text =
                      "Ożesz k..mać... Something wrong on the server! Please contact the administrator to report the issue."
                  };
              default:
                  return null;
          }
      }
  }
```

### atrybuty:
 aby nie eksportowac pola jesli = null
 [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]



### strony z materiałami

<a href="https://www.devtrends.co.uk/blog/handling-errors-in-asp.net-core-web-api" target="_blank">https://www.devtrends.co.uk/blog/handling-errors-in-asp.net-core-web-api</a>

<a href="https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-3.1" target="_blank">https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-3.1</a>

<a href="https://httpstatuses.com/" target="_blank">https://httpstatuses.com/</a>