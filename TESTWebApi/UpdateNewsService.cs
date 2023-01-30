using Microsoft.EntityFrameworkCore;
using System.Threading;
using TESTWebApi;
using TESTWebApi.Models;

public class UpdateNewsService : BackgroundService
{
	private readonly IServiceProvider _IServiceProvider;
	public ParsPage ParsPage;
	private dbConnect dbConnect;
	

	private readonly ILogger<UpdateNewsService> logger;
	public UpdateNewsService(IServiceProvider IServiceProvider,ILogger<UpdateNewsService> logger)
	{
		_IServiceProvider = IServiceProvider;
		this.logger = logger;
	}



	protected async override Task ExecuteAsync(CancellationToken stoppingToken)
	{

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(10000, stoppingToken);
			
			using (var scope = _IServiceProvider.CreateScope())
			{
				ParsPage = new ParsPage();
				dbConnect = scope.ServiceProvider.GetRequiredService<dbConnect>();
				await ParsPage.parsUpdate(dbConnect, stoppingToken);
				
			}
		}


	}

	
}

