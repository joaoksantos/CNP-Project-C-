/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  output: 'export',
  basePath: '/CNP-Project-C-',
  images: {
    unoptimized: true,
  },
  env: {
    NEXT_PUBLIC_API_URL: process.env.NEXT_PUBLIC_API_URL || 'https://projectcnp-h2b5bmh8esajcaar.westus3-01.azurewebsites.net',
  },
}

module.exports = nextConfig
