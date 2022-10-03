using QueueReceiverService;
using QueueReceiverService.Receivers;
using QueueReceiverService.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//builder.Services.AddTransient<IQueueSQSService, ReceiverSQSService>();
//builder.Services.AddTransient<IQueueSBService , ReceiverSBService>();
builder.Services.AddTransient<IQueueReceiverService , ReceiverService>();

builder.Services.AddHostedService<ReceivingWorker>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); 
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

public static class MyGlobalVariables
{
    public static string MyGlobalString { get; set; }
}
