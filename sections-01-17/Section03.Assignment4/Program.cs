using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Hosting;



var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();



app.Run(async (HttpContext context) =>

{

    if (context.Request.Method == "GET" && context.Request.Path == "/")

    {

        var query = context.Request.Query;



        var firstNumberString = query["firstNumber"].ToString();

        var secondNumberString = query["secondNumber"].ToString();

        var operation = query["operation"].ToString();



        var errors = new List<string>();



        if (string.IsNullOrWhiteSpace(firstNumberString))

            errors.Add("Invalid input for 'firstNumber'");



        if (string.IsNullOrWhiteSpace(secondNumberString))

            errors.Add("Invalid input for 'secondNumber'");



        if (string.IsNullOrWhiteSpace(operation))

            errors.Add("Invalid input for 'operation'");



        if (errors.Count > 0)

        {

            context.Response.StatusCode = 400;

            await context.Response.WriteAsync(string.Join("\n", errors));

            return;

        }



        if (!int.TryParse(firstNumberString, out int firstNumber))

        {

            context.Response.StatusCode = 400;

            await context.Response.WriteAsync("Invalid input for 'firstNumber'\n");

            return;

        }



        if (!int.TryParse(secondNumberString, out int secondNumber))

        {

            context.Response.StatusCode = 400;

            await context.Response.WriteAsync("Invalid input for 'secondNumber'\n");

            return;

        }



        try

        {

            var result = operation switch

            {

                "add" => firstNumber + secondNumber,

                "subtract" => firstNumber - secondNumber,

                "multiply" => firstNumber * secondNumber,

                "divide" when secondNumber != 0 => firstNumber / secondNumber,

                "modulo" when secondNumber != 0 => firstNumber % secondNumber,

                _ => throw new ArgumentException("Invalid operation")

            };



            context.Response.StatusCode = 200;

            await context.Response.WriteAsync(result.ToString());

        }

        catch (ArgumentException)

        {

            context.Response.StatusCode = 400;

            await context.Response.WriteAsync("Invalid input for 'operation'");

        }

        catch (DivideByZeroException)

        {

            context.Response.StatusCode = 400;

            await context.Response.WriteAsync("Division by zero is not allowed");

        }

    }

});



app.Run();