// src/components/CadastroCliente.jsx
import React, { useState, useEffect } from 'react';
import './CadastroCliente.css';

function CadastroCliente() {
  const [cliente, setCliente] = useState({
    id: 0,
    NomeCliente: '',
    email: '',
    Telefone: '',
    Endereco: ''
  });
  

  const [clientes, setClientes] = useState([]);
  const [isEditing, setIsEditing] = useState(false);

  const API_URL = 'http://localhost:5239/api/Clientes';

  const fetchClientes = async () => {
    try {
      const response = await fetch(API_URL);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const data = await response.json();
      setClientes(data);
    } catch (error) {
      console.error('Erro ao carregar clientes:', error);
      alert('Erro ao carregar clientes. Verifique o console para mais detalhes.');
    }
  };

  useEffect(() => {
    fetchClientes();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setCliente(prevCliente => ({
      ...prevCliente,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      let response;
      if (isEditing) {
        response = await fetch(`${API_URL}/${cliente.id}`, {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(cliente)
        });
        if (response.status === 204) {
          alert('Cliente atualizado com sucesso!');
        } else if (response.status === 400) {
          const errorData = await response.json();
          alert(`Erro de validação: ${JSON.stringify(errorData.errors)}`);
        } else {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
      } else {
        const clienteParaCriar = { ...cliente };
        delete clienteParaCriar.id; 

        response = await fetch(API_URL, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(clienteParaCriar)
        });
        if (response.status === 201) {
          alert('Cliente cadastrado com sucesso!');
        } else if (response.status === 400) {
          const errorData = await response.json();
          alert(`Erro: ${JSON.stringify(errorData)}`);
        } else {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
      }
      
      resetForm();
      fetchClientes();
    } catch (error) {
      console.error('Erro ao salvar cliente:', error);
      alert('Erro ao salvar cliente. Verifique o console para mais detalhes.');
    }
  };

  const handleEdit = async (id) => {
    try {
      const response = await fetch(`${API_URL}/${id}`);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const clienteToEdit = await response.json();
      setCliente(clienteToEdit);
      setIsEditing(true);
    } catch (error) {
      console.error('Erro ao buscar cliente para edição:', error);
      alert('Erro ao buscar cliente para edição. Verifique o console.');
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Tem certeza que deseja deletar este cliente?')) {
      return;
    }
    try {
      const response = await fetch(`${API_URL}/${id}`, {
        method: 'DELETE'
      });
      if (response.status === 204) {
        alert('Cliente deletado com sucesso!');
        fetchClientes();
      } else if (response.status === 404) {
        alert('Cliente não encontrado.');
      } else {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
    } catch (error) {
      console.error('Erro ao deletar cliente:', error);
      alert('Erro ao deletar cliente. Verifique o console para mais detalhes.');
    }
  };

  const resetForm = () => {
    setCliente({
      id: 0,
      NomeCliente: '',
      email: '',
      Telefone: '',
      endereco: ''
    });
    setIsEditing(false);
  };

  return (
    <div className="container">
      <h1>Cadastro de Clientes</h1>

      <form id="clienteForm" onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="NomeCliente">NomeCliente:</label>
          <input
            type="text"
            id="NomeCliente"
            name="NomeCliente"
            value={cliente.NomeCliente}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="email">Email:</label>
          <input
            type="email"
            id="email"
            name="email"
            value={cliente.email}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="Telefone">Telefone:</label>
          <input
            type="tel"
            id="Telefone"
            name="Telefone"
            value={cliente.Telefone}
            onChange={handleChange}
          />
        </div>

        <div className="form-group">
          <label htmlFor="endereco">Endereço:</label>
          <input
            type="text"
            id="endereco"
            name="endereco"
            value={cliente.endereco}
            onChange={handleChange}
          />
        </div>

        <button type="submit" id="submitBtn">
          {isEditing ? 'Atualizar Cliente' : 'Salvar Cliente'}
        </button>
        {isEditing && (
          <button type="button" id="cancelarEdicao" onClick={resetForm}>
            Cancelar Edição
          </button>
        )}
      </form>

      <hr />

      <h2>Lista de Clientes</h2>
      <div className="table-scroll-container">
        <table id="clientesTable">
          <thead>
            <tr>
              <th>ID</th>
              <th>NomeCliente</th>
              <th>Email</th>
              <th>Telefone</th>
              <th>Endereço</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
  {clientes.map(cli => (
    <tr key={cli.id}>
      <td>{cli.id}</td>
      <td>{cli.NomeCliente}</td>
      <td>{cli.email}</td>
      <td>{cli.Telefone}</td>
      <td>{cli.endereco}</td>
      <td className="actions">
        <button onClick={() => handleEdit(cli.id)}>Editar</button>
        <button className="delete-btn" onClick={() => handleDelete(cli.id)}>Deletar</button>
      </td>
    </tr>
  ))}
            {clientes.length === 0 && (
              <tr>
                <td colSpan="6" style={{ textAlign: 'center', padding: '20px' }}>
                  Nenhum cliente cadastrado.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default CadastroCliente;