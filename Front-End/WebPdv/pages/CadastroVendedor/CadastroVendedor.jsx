import React, { useState, useEffect } from 'react';
import './CadastroVendedor.css';

function CadastroVendedor() {
  const [vendedor, setVendedor] = useState({ id: 0, nomeVendedor: '' });
  const [vendedores, setVendedores] = useState([]);
  const [isEditing, setIsEditing] = useState(false);

  const API_URL = 'http://localhost:5239/api/VendedorControllers';

  const fetchVendedores = async () => {
    try {
      const response = await fetch(API_URL);
      if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
      const data = await response.json();
      setVendedores(data);
    } catch (error) {
      console.error('Erro ao carregar vendedores:', error);
      alert('Erro ao carregar vendedores. Verifique o console para mais detalhes.');
    }
  };

  useEffect(() => {
    fetchVendedores();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setVendedor(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      let response;
      if (isEditing) {
        response = await fetch(`${API_URL}/${vendedor.id}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(vendedor),
        });
        if (response.status === 204) {
          alert('Vendedor atualizado com sucesso!');
        } else if (response.status === 400) {
          const err = await response.json();
          alert(`Erro de validação: ${JSON.stringify(err.errors)}`);
        } else {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
      } else {
        const novoVendedor = { nomeVendedor: vendedor.nomeVendedor };
        response = await fetch(API_URL, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(novoVendedor),
        });
        if (response.status === 201) {
          alert('Vendedor cadastrado com sucesso!');
        } else if (response.status === 400) {
          const err = await response.json();
          alert(`Erro ao cadastrar vendedor: ${JSON.stringify(err)}`);
        } else {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
      }

      resetForm();
      fetchVendedores();
    } catch (error) {
      console.error('Erro ao salvar vendedor:', error);
      alert('Erro ao salvar vendedor. Verifique o console para mais detalhes.');
    }
  };

  const handleEdit = async (id) => {
    try {
      const response = await fetch(`${API_URL}/${id}`);
      if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
      const ven = await response.json();
      setVendedor(ven);
      setIsEditing(true);
    } catch (error) {
      console.error('Erro ao buscar vendedor para edição:', error);
      alert('Erro ao buscar vendedor para edição. Verifique o console.');
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Tem certeza que deseja deletar este vendedor?')) return;
    try {
      const response = await fetch(`${API_URL}/${id}`, { method: 'DELETE' });
      if (response.status === 204) {
        alert('Vendedor deletado com sucesso!');
        fetchVendedores();
      } else if (response.status === 404) {
        alert('Vendedor não encontrado.');
      } else {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
    } catch (error) {
      console.error('Erro ao deletar vendedor:', error);
      alert('Erro ao deletar vendedor. Verifique o console para mais detalhes.');
    }
  };

  const resetForm = () => {
    setVendedor({ id: 0, nomeVendedor: '' });
    setIsEditing(false);
  };

  return (
    <div className="container">
      <h1>Cadastro de Vendedores</h1>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="nomeVendedor">Nome do Vendedor:</label>
          <input
            type="text"
            id="nomeVendedor"
            name="nomeVendedor"
            value={vendedor.nomeVendedor}
            onChange={handleChange}
            required
          />
        </div>
        <button type="submit">{isEditing ? 'Atualizar Vendedor' : 'Salvar Vendedor'}</button>
        {isEditing && <button type="button" onClick={resetForm}>Cancelar Edição</button>}
      </form>

      <hr />

      <h2>Lista de Vendedores</h2>
      <div className="table-scroll-container">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Nome do Vendedor</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {vendedores.length > 0 ? (
              vendedores.map(ven => (
                <tr key={ven.id}>
                  <td>{ven.id}</td>
                  <td>{ven.nomeVendedor}</td>
                  <td>
                    <button onClick={() => handleEdit(ven.id)}>Editar</button>
                    <button onClick={() => handleDelete(ven.id)}>Deletar</button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="3" style={{ textAlign: 'center', padding: '20px' }}>
                  Nenhum vendedor cadastrado.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default CadastroVendedor;