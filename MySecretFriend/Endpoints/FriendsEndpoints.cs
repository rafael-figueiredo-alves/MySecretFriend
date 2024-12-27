using Microsoft.EntityFrameworkCore;
using MySecretFriend.Context;
using MySecretFriend.Entities;
using MySecretFriend.Services;
using MySqlConnector;

namespace MySecretFriend.Endpoints
{
    public static class FriendsEndpoints
    {
        public static void MapFriendEndpoints(this WebApplication app)
        {
            // Endpoint para salvar um registro
            app.MapPost("api/v1/friends", async (FriendsDbContext db, EncryptionService encryption, Friend friend) =>
            {
                try
                {
                    friend.Nome = encryption.Encrypt(friend.Nome);
                    friend.Amigo = encryption.Encrypt(friend.Amigo);
                    db.DbFriends.Add(friend);
                    await db.SaveChangesAsync();
                    return Results.Created($"api/v1/friends/{friend.Id}", friend);
                }
                catch (DbUpdateException ex) when (ex.InnerException is MySqlException mysqlEx && mysqlEx.Number == 1062)
                {
                    // 1062 é o código de erro do MySQL para Duplicate Entry
                    return Results.Conflict(new
                    {
                        Message = "Já existe um registro com o mesmo Nome ou Amigo.",
                        Error = mysqlEx.Message
                    });
                }
                catch (Exception ex)
                {
                    // Tratamento genérico para outros tipos de erros
                    return Results.Problem("Ocorreu um erro ao processar a solicitação.", statusCode: 500);
                }
            });

            // Endpoint para obter a lista de registros embaralhada
            app.MapGet("api/v1/friends", async (FriendsDbContext db, EncryptionService encryption) =>
            {
                var friends = await db.DbFriends.ToListAsync();
                var result = friends.Select(f => new Friend
                {
                    Id = f.Id,
                    Nome = encryption.Decrypt(f.Nome),
                    Amigo = encryption.Decrypt(f.Amigo),
                    Dica1 = f.Dica1,
                    Dica2 = f.Dica2,
                    Dica3 = f.Dica3,
                    Dica4 = f.Dica4
                }).OrderBy(_ => Guid.NewGuid()).ToList();

                return Results.Ok(result);
            });

            app.MapDelete("api/v1/friends", async (FriendsDbContext db) =>
            {
                await db.DbFriends.ExecuteDeleteAsync();
            });
        }
    }
}
