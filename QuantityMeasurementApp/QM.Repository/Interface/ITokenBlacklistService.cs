namespace QM.Repository.Interface;

public interface ITokenBlacklistService
{
    void Revoke(string token);
    bool IsRevoked(string token);
}