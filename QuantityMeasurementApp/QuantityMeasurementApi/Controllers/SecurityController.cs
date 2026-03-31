using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QM.Models.DTOs;
// using QuantityMeasurementApi.Services;
using QM.BusinessLogic.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace QuantityMeasurementApi.Controllers;

[ApiController]
[Route("api/v1/security")]
[Authorize]
[Tags("Security - Encryption & Hashing")]
public class SecurityController : ControllerBase
{
    private readonly ICryptoService _crypto;
    private readonly IHashService   _hash;

    public SecurityController(ICryptoService crypto, IHashService hash)
    {
        _crypto = crypto;
        _hash   = hash;
    }

    [HttpPost("encrypt")]
    [SwaggerOperation(Summary = "AES-256 encrypt plaintext")]
    public IActionResult Encrypt([FromBody] EncryptRequestDTO dto) =>
        Ok(new EncryptResponseDTO(_crypto.Encrypt(dto.PlainText)));

    [HttpPost("decrypt")]
    [SwaggerOperation(Summary = "AES-256 decrypt ciphertext")]
    public IActionResult Decrypt([FromBody] DecryptRequestDTO dto) =>
        Ok(new DecryptResponseDTO(_crypto.Decrypt(dto.CipherText)));

    [HttpPost("hash/sha256")]
    [SwaggerOperation(Summary = "Hash input with SHA-256")]
    public IActionResult HashSha256([FromBody] HashRequestDTO dto) =>
        Ok(new HashResponseDTO(_hash.HashSha256(dto.Input), "SHA-256"));

    [HttpPost("hash/sha512")]
    [SwaggerOperation(Summary = "Hash input with SHA-512")]
    public IActionResult HashSha512([FromBody] HashRequestDTO dto) =>
        Ok(new HashResponseDTO(_hash.HashSha512(dto.Input), "SHA-512"));

    [HttpPost("hash/bcrypt")]
    [SwaggerOperation(Summary = "Hash password with BCrypt")]
    public IActionResult HashBcrypt([FromBody] HashRequestDTO dto) =>
        Ok(new HashResponseDTO(_hash.HashBcrypt(dto.Input), "BCrypt"));

    [HttpPost("hash/verify/bcrypt")]
    [SwaggerOperation(Summary = "Verify BCrypt hash")]
    public IActionResult VerifyBcrypt([FromBody] HashVerifyDTO dto) =>
        Ok(new { IsValid = _hash.VerifyBcrypt(dto.Input, dto.Hash) });

    [HttpPost("hash/verify/sha256")]
    [SwaggerOperation(Summary = "Verify SHA-256 hash")]
    public IActionResult VerifySha256([FromBody] HashVerifyDTO dto) =>
        Ok(new { IsValid = _hash.VerifySha256(dto.Input, dto.Hash) });
}