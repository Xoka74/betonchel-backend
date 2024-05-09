using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;

namespace Betonchel.Api.Utils;

public class DataSeeder
{
    private readonly IFilterableRepository<User, int> _repository;
    private readonly IConfiguration _configuration;

    public DataSeeder(IFilterableRepository<User, int> repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task SeedDataAsync()
    {
        if (_repository.GetAll().Any()) return;
        
        var user = new User
        {
            Email = _configuration["Admin:Email"],
            FullName = _configuration["Admin:FullName"]
        };
        
        await _repository.Create(user);
    }
}