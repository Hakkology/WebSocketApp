using WebSocketApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<WebSocketModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseWebSockets();

app.MapWhen(
    context => context.WebSockets.IsWebSocketRequest && context.Request.Path == "/ws",
    appBranch => {
        appBranch.Use(async (context, next) => 
        {
            var webSocketHandler = context.RequestServices.GetRequiredService<WebSocketHandler>();
            await webSocketHandler.Handle(context, next);
        });
    });

    
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
