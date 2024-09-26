import React, { useState } from 'react';
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Input,
  Stack,
  Heading,
  Text,
  useToast,
} from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';

interface LoginRequest {
  email: string | null;
  password: string | null;
  twoFactorCode?: string | null;
  twoFactorRecoveryCode?: string | null;
}

const Login: React.FC = () => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [twoFactorCode, setTwoFactorCode] = useState<string | null>(null);
  const [twoFactorRecoveryCode, setTwoFactorRecoveryCode] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const toast = useToast();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setLoading(true);

    const loginRequest: LoginRequest = {
      email: email || null,
      password: password || null,
      twoFactorCode: twoFactorCode || null,
      twoFactorRecoveryCode: twoFactorRecoveryCode || null,
    };

    try {
      const response = await fetch('http://localhost:5237/identity/login?useCookies=true', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(loginRequest),
        credentials: 'include',
      });

      console.log('Response status:', response.status);

      const responseText = await response.text();

      if (!response.ok) {
        console.error('Error response body:', responseText);
        throw new Error('Login failed: ' + responseText);
      }

    //   let data;
    //   try {
    //     data = JSON.parse(responseText);
    //   } catch (error) {
    //     console.error('Failed to parse JSON:', responseText);
    //     throw new Error('Login failed: Invalid JSON response');
    //   }

    // Set the access token in a cookie
    //   Cookies.set('.AspNetCore.Identity.Application', data.accessToken, { expires: 1 }); // Expires in 1 day

      console.log('Login successful!');
      toast({
        title: 'Login successful.',
        description: 'Welcome back!',
        status: 'success',
        duration: 5000,
        isClosable: true,
      });

      navigate('/');
    } catch (error) {
      toast({
        title: 'Login failed.',
        description: (error as Error).message,
        status: 'error',
        duration: 5000,
        isClosable: true,
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box maxW="lg" mx="auto" mt={10} p={5} borderWidth={1} borderRadius="lg">
      <Heading mb={6}>Login</Heading>
      <form onSubmit={handleSubmit}>
        <Stack spacing={4}>
          <FormControl id="email" isRequired>
            <FormLabel>Email</FormLabel>
            <Input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Enter your email"
            />
          </FormControl>

          <FormControl id="password" isRequired>
            <FormLabel>Password</FormLabel>
            <Input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Enter your password"
            />
          </FormControl>

          <Button
            type="submit"
            colorScheme="teal"
            isLoading={loading}
            loadingText="Logging in"
          >
            Login
          </Button>
        </Stack>
      </form>
      <Text mt={4}>
        Don't have an account? <a href="/register">Register</a>
      </Text>
    </Box>
  );
};

export default Login;
