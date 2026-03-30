namespace QM.Models.DTOs;

public record RegisterDTO(string Username, string Email, string Password);
public record LoginDTO(string Email, string Password);
public record AuthResponseDTO(string Token, string TokenType = "Bearer");
public record EncryptRequestDTO(string PlainText);
public record EncryptResponseDTO(string CipherText);
public record DecryptRequestDTO(string CipherText);
public record DecryptResponseDTO(string PlainText);
public record HashRequestDTO(string Input);
public record HashResponseDTO(string Hash, string Algorithm);
public record HashVerifyDTO(string Input, string Hash);