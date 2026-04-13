
export const maskCpf = (cpf: string) => {
  const digitsOnly = cpf.replace(/\D/g, '')
  if (digitsOnly.length > 11) return cpf // Não processa se tiver mais de 11 dígitos
  return `***.***.**-${digitsOnly.slice(-2)}`
}