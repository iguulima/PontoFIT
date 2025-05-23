import React, { useState, useEffect } from 'react';
import CreateUsuario from './components/CreateUsuario';

export default function App() {
  const [usuarios, setUsuarios] = useState([]);

  useEffect(() => {
    fetchUsuarios();
  }, []);

  const fetchUsuarios = async () => {
    const res = await fetch('http://localhost:5159/usuarios');
    const data = await res.json();
    setUsuarios(data);
  };

  return (
    <div>
      <CreateUsuario onUsuarioCriado={fetchUsuarios} />
      <h2>Lista de Usuários</h2>
      <ul>
        {usuarios.map(u => (
          <li key={u.id}>
            {u.nome} ({u.username}) - Tipo: {u.tipoUsuario}
          </li>
        ))}
      </ul>
    </div>
  );
}
