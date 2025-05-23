import React, { useState } from 'react';

export default function CreateUsuario({ onUsuarioCriado }) {
  const [nome, setNome] = useState('');
  const [username, setUsername] = useState('');
  const [senha, setSenha] = useState('');
  const [tipoUsuario, setTipoUsuario] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();

    const novoUsuario = {
      nome,
      username,
      senha,
      tipoUsuario,
    };

    try {
        const response = await fetch('http://localhost:5159/api/usuarios', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(novoUsuario),
      });

      if (!response.ok) {
        throw new Error('Erro ao criar usuário');
      }

      const usuarioCriado = await response.json();

      // Limpa os campos
      setNome('');
      setUsername('');
      setSenha('');
      setTipoUsuario('');

      // Callback para avisar o componente pai que um usuário novo foi criado
      if (onUsuarioCriado) onUsuarioCriado(usuarioCriado);
      
      alert('Usuário criado com sucesso!');
    } catch (error) {
      alert(error.message);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <h3>Criar Novo Usuário</h3>

      <input
        type="text"
        placeholder="Nome"
        value={nome}
        onChange={e => setNome(e.target.value)}
        required
      />
      <br />

      <input
        type="text"
        placeholder="Username"
        value={username}
        onChange={e => setUsername(e.target.value)}
        required
      />
      <br />

      <input
        type="password"
        placeholder="Senha"
        value={senha}
        onChange={e => setSenha(e.target.value)}
        required
      />
      <br />

      <input
        type="text"
        placeholder="Tipo de Usuário"
        value={tipoUsuario}
        onChange={e => setTipoUsuario(e.target.value)}
        required
      />
      <br />

      <button type="submit">Criar Usuário</button>
    </form>
  );
}
