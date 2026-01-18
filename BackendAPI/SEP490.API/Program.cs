namespace SEP490.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Thêm dịch vụ Rate Limiting
            builder.Services.AddRateLimiter(options =>
            {
                // Cấu hình chính sách "fixed" (cố định)
                options.AddFixedWindowLimiter(policyName: "fixed", opt =>
                {
                    opt.PermitLimit = 10; // Tối đa 10 yêu cầu
                    opt.Window = TimeSpan.FromSeconds(10); // Trong vòng 10 giây
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 2; // Số lượng yêu cầu chờ trong hàng đợi
                });

                // Cấu hình phản hồi khi bị giới hạn (429 Too Many Requests)
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            // Add services to the container.
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
