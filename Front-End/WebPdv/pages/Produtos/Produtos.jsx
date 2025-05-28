// src/Produtos.jsx
import React, { useState, useEffect } from "react";
import './Produtos.css';

function Produtos() {
    const [produtos, setProdutos] = useState([]);
    const [produtoSelecionado, setProdutoSelecionado] = useState(null);

    const API_BASE_URL = "https://localhost:5239/api/Produtos"; 

    const carregarProdutos = async () => {
        try {
            const response = await fetch(API_BASE_URL);
            if (!response.ok) {
                throw new Error(`Erro HTTP: ${response.status}`);
            }
            const data = await response.json();
            setProdutos(data);
        } catch (error) {
            console.error("Erro ao carregar produtos:", error);
            alert("Não foi possível carregar os produtos. Verifique a conexão com a API.");
        }
    };

    useEffect(() => {
        carregarProdutos();
    }, []); 

    const handleSaveProduto = async (produto) => {
        try {
            let response;
            if (produto.id === 0) {
                response = await fetch(API_BASE_URL, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(produto),
                });
            } else {
                response = await fetch(`${API_BASE_URL}/${produto.id}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(produto),
                });
            }

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(`Erro ao salvar produto: ${response.status} - ${errorData.title || JSON.stringify(errorData)}`);
            }

            carregarProdutos();
            setProdutoSelecionado(null);
            alert(`Produto ${produto.id === 0 ? 'cadastrado' : 'atualizado'} com sucesso!`);
        } catch (error) {
            console.error("Erro ao salvar produto:", error);
            alert(`Erro ao salvar produto: ${error.message}`);
        }
    };

    const handleEditProduto = (produto) => {
        setProdutoSelecionado(produto);
    };

    const handleDeleteProduto = async (id) => {
        if (window.confirm("Tem certeza que deseja deletar este produto?")) {
            try {
                const response = await fetch(`${API_BASE_URL}/${id}`, {
                    method: 'DELETE',
                });

                if (!response.ok) {
                    throw new Error(`Erro HTTP: ${response.status}`);
                }

                carregarProdutos();
                alert("Produto deletado com sucesso!");
            } catch (error) {
                console.error("Erro ao deletar produto:", error);
                alert("Não foi possível deletar o produto.");
            }
        }
    };

    function ProdutoForm({ produtoParaEditar, onSave }) {
        const [produto, setProduto] = useState({
            id: 0,
            nome: '',
            preco: 0,
            quantidade: 0,
        });

        useEffect(() => {
            if (produtoParaEditar) {
                setProduto(produtoParaEditar);
            } else {
                setProduto({ id: 0, nome: '', preco: 0, quantidade: 0 });
            }
        }, [produtoParaEditar]);

        const handleChange = (e) => {
            const { name, value, type } = e.target;
            setProduto(prevProduto => ({
                ...prevProduto,
                [name]: type === 'number' ? parseFloat(value) : value
            }));
        };

        const handleSubmit = async (e) => {
            e.preventDefault();
            onSave(produto);
        };

        return (
            <div className="produto-form-container">
                <h2>{produto.id === 0 ? 'Cadastrar Novo Produto' : `Editar Produto: ${produto.nome}`}</h2>
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="nome">Nome:</label>
                        <input
                            type="text"
                            id="nome"
                            name="nome"
                            value={produto.nome}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="preco">Preço:</label>
                        <input
                            type="number"
                            id="preco"
                            name="preco"
                            value={produto.preco}
                            onChange={handleChange}
                            step="0.01"
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="quantidade">Quantidade:</label>
                        <input
                            type="number"
                            id="quantidade"
                            name="quantidade"
                            value={produto.quantidade}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <button type="submit" className="btn-submit">
                        {produto.id === 0 ? 'Cadastrar' : 'Salvar Edição'}
                    </button>
                </form>
            </div>
        );
    }

    function ProdutoList({ produtos, onEdit, onDelete }) {
        return (
            <div className="produto-list-container">
                <h2>Lista de Produtos</h2>
                {produtos.length === 0 ? (
                    <p>Nenhum produto cadastrado ainda.</p>
                ) : (
                    <ul className="produto-list">
                        {produtos.map(produto => (
                            <li key={produto.id} className="produto-item">
                                <div className="produto-info">
                                    <h3>{produto.nome}</h3>
                                    <p>Preço: R$ {produto.preco.toFixed(2)}</p>
                                    <p>Quantidade: {produto.quantidade}</p>
                                </div>
                                <div className="produto-actions">
                                    <button
                                        onClick={() => onEdit(produto)}
                                        className="btn-edit"
                                    >
                                        Editar
                                    </button>
                                    <button
                                        onClick={() => onDelete(produto.id)}
                                        className="btn-delete"
                                    >
                                        Deletar
                                    </button>
                                </div>
                            </li>
                        ))}
                    </ul>
                )}
            </div>
        );
    }

    return (
        <div className="produtos-page-container">
            <h1>Gestão de Produtos</h1>
            <ProdutoForm 
                produtoParaEditar={produtoSelecionado} 
                onSave={handleSaveProduto} 
            />
            <hr />
            <ProdutoList 
                produtos={produtos} 
                onEdit={handleEditProduto} 
                onDelete={handleDeleteProduto} 
            />
        </div>
    );
}

export default Produtos;