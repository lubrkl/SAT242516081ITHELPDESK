namespace DbContexts;

using Microsoft.EntityFrameworkCore;

public class MyDbModel_DbContext(DbContextOptions<MyDbModel_DbContext> options) : DbContext(options)
{
}