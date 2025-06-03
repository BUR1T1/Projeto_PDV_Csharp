import React, { useState, useEffect } from "react";
import './Produtos.css';

function Produtos() {
    const [produtos, setProdutos] = useState([]);
    const [produtoSelecionado, setProdutoSelecionado] = useState(null);

    const API_BASE_URL = "http://localhost:5239/api/Produtos"; 

    // Função para carregar produtos do backend
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

    // useEffect para carregar os produtos assim que o componente é montado
    useEffect(() => {
        carregarProdutos();
    }, []); 

    const handleSaveProduto = async (produto) => {
        try {
            let response;
            // Verifica se é um novo produto (id === 0) ou uma edição
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

            // Recarrega a lista de produtos após salvar
            carregarProdutos();
          
            setProdutoSelecionado(null);
            alert(`Produto ${produto.id === 0 ? 'cadastrado' : 'atualizado'} com sucesso!`);
        } catch (error) {
            console.error("Erro ao salvar produto:", error);
            alert(`Erro ao salvar produto: ${error.message}`);
        }
    };

    // Função para editar um produto (preenche o formulário com os dados do produto)
    const handleEditProduto = (produto) => {
        setProdutoSelecionado(produto);
    };

    // Função para deletar um produto
    const handleDeleteProduto = async (id) => {
        if (window.confirm("Tem certeza que deseja deletar este produto?")) {
            try {
                const response = await fetch(`${API_BASE_URL}/${id}`, {
                    method: 'DELETE',
                });

                if (!response.ok) {
                    throw new Error(`Erro HTTP: ${response.status}`);
                }

                // Recarrega a lista de produtos após deletar
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
            nomeProduto: '', 
            valorDeVenda: 0,
            quantidedeDeEstoque: 0, 
            grupoProdutos: '' 
        });

        // Atualiza o estado do formulário quando um produto para editar é selecionado
        useEffect(() => {
            if (produtoParaEditar) {
                setProduto(produtoParaEditar);
            } else {
                // Reseta o formulário para um novo produto
                setProduto({ id: 0, nomeProduto: '', valorDeVenda: 0, quantidedeDeEstoque: 0, grupoProdutos: '' });
            }
        }, [produtoParaEditar]);

        // Lida com a mudança nos campos do formulário
        const handleChange = (e) => {
            const { name, value, type } = e.target;
            setProduto(prevProduto => ({
                ...prevProduto,
                [name]: type === 'number' ? parseFloat(value) : value
            }));
        };

        // Lida com o envio do formulário
        const handleSubmit = async (e) => {
            e.preventDefault();
            onSave(produto);
        };

        return (
            <div className="produto-form-container">
                <h2>{produto.id === 0 ? 'Cadastrar Novo Produto' : `Editar Produto: ${produto.nomeProduto}`}</h2>
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="nomeProduto">Nome:</label>
                        <input
                            type="text"
                            id="nomeProduto"
                            name="nomeProduto"
                            value={produto.nomeProduto} 
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="valorDeVenda">Preço de Venda:</label>
                        <input
                            type="number"
                            id="valorDeVenda"
                            name="valorDeVenda" 
                            value={produto.valorDeVenda} 
                            onChange={handleChange}
                            step="0.01"
                            required
                        />
                    </div>
                     <div className="form-group">
                        <label htmlFor="valorDeCusto">Preço de Custo:</label>
                        <input
                            type="number"
                            id="valorDeCusto"
                            name="valorDeCusto" 
                            value={produto.valorDeCusto || 0} 
                            onChange={handleChange}
                            step="0.01"
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="quantidedeDeEstoque">Quantidade:</label>
                        <input
                            type="number"
                            id="quantidedeDeEstoque"
                            name="quantidedeDeEstoque" 
                            value={produto.quantidedeDeEstoque} 
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="grupoProdutos">Grupo de Produtos:</label>
                        <input
                            type="text"
                            id="grupoProdutos"
                            name="grupoProdutos"
                            value={produto.grupoProdutos}
                            onChange={handleChange}
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
                                    <h3>{produto.nomeProduto}</h3> 
                                    <p>Preço de Venda: R$ {Number(produto.valorDeVenda || 0).toFixed(2)}</p> 
                                    <p>Preço de Custo: R$ {Number(produto.valorDeCusto || 0).toFixed(2)}</p> 
                                    <p>Quantidade: {produto.quantidedeDeEstoque}</p> 
                                    <p>Grupo: {produto.grupoProdutos || 'N/A'}</p> 
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