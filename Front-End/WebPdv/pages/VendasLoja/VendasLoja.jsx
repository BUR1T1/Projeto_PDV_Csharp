import React, { useState, useEffect } from 'react';
import './VendasLoja.css';

function VendasLoja() {
  const [vendas, setVendas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [editingVenda, setEditingVenda] = useState(null);

  const [formData, setFormData] = useState({
    valorDaVenda: 0,
    itensDaVenda: []
  });

  const API_BASE_URL = 'http://localhost:5239/api/Vendas';

  const fetchVendas = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await fetch(API_BASE_URL);
      if (!response.ok) {
        throw new Error(`Erro HTTP! Status: ${response.status}`);
      }
      const data = await response.json();
      setVendas(data);
    } catch (err) {
      console.error('Erro ao buscar vendas:', err);
      setError('Não foi possível carregar as vendas. Tente novamente mais tarde.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchVendas();
  }, []);

  const handleFormChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value
    }));
  };

  const handleItemChange = (index, e) => {
    const { name, value } = e.target;
    const newItems = [...formData.itensDaVenda];
    newItems[index] = {
      ...newItems[index],
      [name]: name === 'quantidade' || name === 'produtoId' ? parseInt(value) : value
    };
    setFormData((prev) => ({
      ...prev,
      itensDaVenda: newItems
    }));
  };

  const handleAddItem = () => {
    setFormData((prev) => ({
      ...prev,
      itensDaVenda: [...prev.itensDaVenda, { produtoId: '', quantidade: '' }]
    }));
  };

  const handleRemoveItem = (index) => {
    setFormData((prev) => ({
      ...prev,
      itensDaVenda: prev.itensDaVenda.filter((_, i) => i !== index)
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      if (!editingVenda) {
        throw new Error('Erro: Tentativa de salvar venda sem estar no modo de edição.');
      }

      const payload = {
        id: editingVenda.id,
        itensDaVenda: formData.itensDaVenda.map((item) => ({
          produtoId: parseInt(item.produtoId),
          quantidade: parseInt(item.quantidade)
        }))
      };

      const response = await fetch(`${API_BASE_URL}/${editingVenda.id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData.title || errorData.detail || 'Falha ao atualizar venda!');
      }

      alert('Venda atualizada com sucesso!');
      setEditingVenda(null);
      setFormData({ valorDaVenda: 0, itensDaVenda: [] });
      fetchVendas();
    } catch (err) {
      console.error('Erro ao salvar venda:', err);
      setError(`Erro ao salvar venda: ${err.message}`);
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (venda) => {
    setEditingVenda(venda);

    setFormData({
      valorDaVenda: venda.valorDaVenda,
      itensDaVenda: venda.itensDaVenda.map((item) => ({
        produtoId: item.produtoId,
        quantidade: item.quantidade,
        nomeProduto: item.nomeProduto,
        valorUnitario: item.valorUnitario
      }))
    });
  };

  const handleCancel = () => {
    setEditingVenda(null);
    setFormData({ valorDaVenda: 0, itensDaVenda: [] });
    setError(null);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Tem certeza que deseja deletar esta venda?')) {
      return;
    }

    setLoading(true);
    setError(null);
    try {
      const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE'
      });

      if (!response.ok) {
        throw new Error(`Erro ao deletar! Status: ${response.status}`);
      }

      alert('Venda deletada com sucesso!');
      fetchVendas();
    } catch (err) {
      console.error('Erro ao deletar venda:', err);
      setError('Não foi possível deletar a venda. Tente novamente mais tarde.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="vendas-container">
      <h2>Vendas da Loja</h2>

      {editingVenda && (
        <div className="venda-form-card">
          <h3>Editar Venda (ID: {editingVenda.id})</h3>
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label htmlFor="valorDaVenda">Valor Total da Venda:</label>
              <input
                type="number"
                id="valorDaVenda"
                name="valorDaVenda"
                value={formData.valorDaVenda.toFixed(2)}
                readOnly
              />
            </div>

            <h4>Itens da Venda</h4>
            {formData.itensDaVenda.map((item, index) => (
              <div key={index} className="item-venda-group">
                <div className="form-group">
                  <label htmlFor={`produtoId-${index}`}>ID do Produto:</label>
                  <input
                    type="number"
                    id={`produtoId-${index}`}
                    name="produtoId"
                    value={item.produtoId}
                    onChange={(e) => handleItemChange(index, e)}
                    required
                  />
                </div>
                <div className="form-group">
                  <label htmlFor={`quantidade-${index}`}>Quantidade:</label>
                  <input
                    type="number"
                    id={`quantidade-${index}`}
                    name="quantidade"
                    value={item.quantidade}
                    onChange={(e) => handleItemChange(index, e)}
                    required
                  />
                </div>
                <button type="button" onClick={() => handleRemoveItem(index)} className="remove-item-btn">
                  Remover Item
                </button>
              </div>
            ))}
            <button type="button" onClick={handleAddItem} className="add-item-btn">
              Adicionar Novo Item
            </button>

            <div className="form-actions">
              <button type="submit" className="submit-btn" disabled={loading}>
                {loading ? 'Salvando...' : 'Atualizar Venda'}
              </button>
              <button type="button" onClick={handleCancel} className="cancel-btn">
                Cancelar
              </button>
            </div>
          </form>
        </div>
      )}

      {loading && <p>Carregando vendas...</p>}
      {error && <p className="error-message">{error}</p>}

      {!loading && !error && vendas.length === 0 && !editingVenda && <p>Nenhuma venda encontrada.</p>}

      {!loading && !error && vendas.length > 0 && (
        <div className="vendas-lista">
          <h3>Lista de Vendas</h3>
          <table>
            <thead>
              <tr>
                <th>ID Venda</th>

                <th>Valor Total</th>
                <th>Itens</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {vendas.map((venda) => (
                <tr key={venda.id}>
                  <td>{venda.id}</td>

                  <td>{venda.valorDaVenda.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                  <td>
                    <ul className="itens-lista">
                      {venda.itensDaVenda.map((item) => (
                        <li key={item.id}>
                          {item.nomeProduto} ({item.quantidade}x) -{' '}
                          {item.valorUnitario.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })} cada
                        </li>
                      ))}
                    </ul>
                  </td>
                  <td className="actions-cell">
                    <button onClick={() => handleEdit(venda)} className="edit-btn">
                      Editar
                    </button>
                    <button onClick={() => handleDelete(venda.id)} className="delete-btn">
                      Deletar
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}

export default VendasLoja;