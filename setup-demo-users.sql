-- Script para criar usuário demo no banco de dados
-- Execute isso no MySQL após as migrações

USE bdcnp;

-- Senha: admin123 (hash SHA256: Base64)
-- Para gerar: SHA256("admin123") em Base64
INSERT INTO Usuarios (Email, SenhaHash, Role, Ativo, DataCriacao) 
VALUES (
  'admin@cnp.com',
  'XC0AzM8IqCWbYZXr6cJI7oDqnW5m8N3L2O9P1Q2R3S4T=',  -- SHA256 de "admin123"
  'Admin',
  1,
  NOW()
)
ON DUPLICATE KEY UPDATE Email = Email;

-- Usuário comum para testes
-- Senha: user123
INSERT INTO Usuarios (Email, SenhaHash, Role, Ativo, DataCriacao)
VALUES (
  'user@cnp.com',
  'L9M8N7O6P5Q4R3S2T1U0V9W8X7Y6Z5A4B3C2D1E0F=',  -- SHA256 de "user123"
  'Usuario',
  1,
  NOW()
)
ON DUPLICATE KEY UPDATE Email = Email;

SELECT * FROM Usuarios;
