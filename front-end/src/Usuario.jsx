import React, { useState, useEffect } from 'react';

export default function Usuarios() {
  const [usuarios, setUsuarios] = useState([]);
  const [nome, setNome] = useState('');
  const [username, setUsername] = useState('');
  const [senha, setSenha] = useState('');
  const [tipoUsuario, setTipoUsuario] = useState('');

  useEffect(() => {
    fetchUsuarios();
  }, []);

  function fetchUsuarios() {
    fetch('http://localhost:5159/Usuarios')
      .then(res => res.json())
      .then(data => setUsuarios(data))
      .catch(err => console.error('Erro ao buscar usuários:', err));
  }

  function handleSubmit(e) {
    e.preventDefault();

    const novoUsuario = { nome, username, senha, tipoUsuario };

    fetch('http://localhost:5159/Usuarios', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(novoUsuario)
    })
      .then(res => {
        if (!res.ok) throw new Error('Erro ao criar usuário');
        return res.json();
      })
      .then(data => {
        setUsuarios([...usuarios, data]);
        // limpa formulário
        setNome('');
        setUsername('');
        setSenha('');
        setTipoUsuario('');
      })
      .catch(err => alert(err.message));
  }

  return (
    <div>
      <h2>Lista de Usuários</h2>
      <ul>
        {usuarios.map(usuario => (
          <li key={usuario.id}>
            {usuario.nome} ({usuario.username}) - Tipo: {usuario.tipoUsuario}
          </li>
        ))}
      </ul>

      <h3>Adicionar Usuário</h3>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Nome"
          value={nome}
          onChange={e => setNome(e.target.value)}
          required
        />
        <input
          type="text"
          placeholder="Username"
          value={username}
          onChange={e => setUsername(e.target.value)}
          required
        />
        <input
          type="password"
          placeholder="Senha"
          value={senha}
          onChange={e => setSenha(e.target.value)}
          required
        />
        <input
          type="text"
          placeholder="Tipo de Usuário"
          value={tipoUsuario}
          onChange={e => setTipoUsuario(e.target.value)}
          required
        />
        <button type="submit">Criar</button>
      </form>
    </div>
  );
}
