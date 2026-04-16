namespace AnroveBPO.Contracts.Auth.Requests;

public record LoginRequest(
    string UserName,
    string Password);
