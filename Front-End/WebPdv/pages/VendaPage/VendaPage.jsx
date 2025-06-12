import React, { useState } from 'react';
import styles from './VendaPage.module.css';
import axios from 'axios';

function VendaPage() {
  const [cliente, setCliente] = useState('');
  const [vendedor, setVendedor] = useState('');
  const [formaDePagamento, setFormaDePagamento] = useState('Dinheiro'); 
  const [produtoId, setProdutoId] = useState('');
  const [quantidade, setQuantidade] = useState(1);
  const [itens, setItens] = useState([]);
  const [valorTotal, setValorTotal] = useState(0);
  const [ValorUnitario, setValorUnitario] = useState();

  const adicionarItem = async (e) => {
    if (e.key === 'Enter' && produtoId) {
      try {
        const res = await axios.get(`http://localhost:5239/api/produtos/${produtoId}`);
        const produto = res.data;

        const novoItem = {
          ProdutoId: produto.id,
          NomeProduto: produto.nomeProduto,
          Quantidade: quantidade,
          ValorUnitario: produto.valorDeVenda,
          Subtotal: (produto.valorDeVenda * quantidade).toFixed(2)
        };

        setItens([...itens, novoItem]);
        setValorTotal(prev => prev + produto.valorDeVenda * quantidade);
        setProdutoId('');
        setQuantidade(1);
      } catch (error) {
        console.error("Erro ao buscar produto:", error); 
        alert("Erro ao buscar produto: " + (error.response?.data?.message || error.response?.data || error.message || "Verifique o console para mais detalhes."));
      }
    }
  };

  const limparVenda = () => {
    setCliente('');
    setVendedor('');
    setFormaDePagamento('Dinheiro');
    setProdutoId('');
    setQuantidade(1);
    setItens([]);
    setValorTotal(0);
  };

  const deleteItem = (indexToRemove) => {
    const itemRemovido = itens[indexToRemove];
    const novosItens = itens.filter((_, index) => index !== indexToRemove);
  
    setItens(novosItens);
    setValorTotal(prev => prev - itemRemovido.ValorUnitario * itemRemovido.Quantidade);
  };  

  const finalizarVenda = async () => {
    if (!cliente || !vendedor || itens.length === 0) {
      alert("Preencha os campos obrigatórios e adicione pelo menos um item.");
      return;
    }

    try {
      const venda = {
        NomeCliente: cliente,
        NomeVendedor: vendedor,
        FormaDePagamento: formaDePagamento,
        ItensDaVenda: itens.map(item => ({
          ProdutoId: item.ProdutoId,
          Quantidade: item.Quantidade
        }))
      };

      await axios.post('http://localhost:5239/api/vendas', venda);
      alert('Venda registrada com sucesso!');
      limparVenda();
    } catch (error) {
      console.error("Erro completo ao finalizar venda:", error.response); 
      console.error("Dados da resposta de erro:", error.response?.data); 

      let errorMessage = "Erro desconhecido ao finalizar venda.";

      if (error.response) {
        if (typeof error.response.data === 'string') {
          errorMessage = error.response.data; 
        } else if (error.response.data && typeof error.response.data === 'object') {
          if (error.response.data.errors) { 
            errorMessage = "Erro de validação: " + Object.values(error.response.data.errors).flat().join('; ');
          } else if (error.response.data.message) { 
            errorMessage = error.response.data.message;
          } else {
            errorMessage = "Erro detalhado (veja o console): " + JSON.stringify(error.response.data, null, 2);
          }
        } else if (error.message) {
            errorMessage = error.message;
        }
      } else if (error.message) {
        errorMessage = error.message; 
      }

      alert("Erro ao finalizar venda: " + errorMessage);
    }
  };

  return (
    <div className={styles.parent}>
      <div className={styles.div1}>
        <input
          type="text"
          placeholder="Buscar produto por ID"
          value={produtoId}
          onChange={e => setProdutoId(e.target.value)}
          onKeyDown={adicionarItem}
          className={styles.input}
        />
      </div>

      <div className={styles.div2}>
        <div className={styles.infoContainer}>
          <input
            type="text"
            placeholder="Cliente"
            value={cliente}
            onChange={e => setCliente(e.target.value)}
            className={styles.infoInput}
          />
          <input
            type="text"
            placeholder="Vendedor"
            value={vendedor}
            onChange={e => setVendedor(e.target.value)}
            className={styles.infoInput}
          />
          <input
            type="text"
            placeholder="Total"
            value={`R$ ${valorTotal.toFixed(2)}`}
            readOnly
            className={styles.infoInput}
          />
          <select
            value={formaDePagamento}
            onChange={e => setFormaDePagamento(e.target.value)}
            className={styles.selectPagamento}
          >
            <option value="Dinheiro">Dinheiro</option>
            <option value="Cartão de Crédito">Cartão de Crédito</option>
            <option value="Cartão de Débito">Cartão de Débito</option>
            <option value="Pix">Pix</option>
          </select>

        </div>
      </div>

      <div className={styles.div3}>
        <ul style={{ listStyle: 'none', padding: 0, margin: 0, width: '100%' }}>
          {itens.map((item, index) => (
            <li key={index} style={{ marginBottom: '4px' }}>
              {item.NomeProduto} x{item.Quantidade} = R$ {item.Subtotal}
              <button className={styles.button} onClick={() => deleteItem(index)}>Remover</button></li>
          ))}
        </ul>
      </div>

    
      <div className={styles.div5}>
        <button className={styles.button} onClick={limparVenda}>
          Limpar
        </button>
      </div>

      <div className={styles.div6}>
        <button className={styles.button} onClick={finalizarVenda}>
          Finalizar
        </button>
      </div>
    </div>
  );
}

export default VendaPage;