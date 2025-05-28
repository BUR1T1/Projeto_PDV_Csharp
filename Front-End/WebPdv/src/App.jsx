import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Header from '/components/Header.jsx';
import CadastroCliente from '/pages/CadastroCliente/CadastroCliente.jsx';
import CadastroVendedor from '/pages/CadastroVendedor/CadastroVendedor.jsx';
import VendasLoja from '/pages/VendasLoja/VendasLoja.jsx';

import VendaPage from '/pages/VendaPage/VendaPage.jsx'; 
import Produtos from '../pages/Produtos/Produtos';


function App() {
  return (
    <Router>
      <div className="App">
        <Header />
        <main style={{ padding: '20px' }}>
          <Routes>
           
            <Route path="/cadastro-cliente" element={<CadastroCliente />} />
            <Route path="/cadastro-vendedor" element={<CadastroVendedor />} />
            <Route path="/vendas-loja" element={<VendasLoja />} />
            <Route path="/venda-page" element={<VendaPage />} /> 
            <Route path= "/Produtos" element={<Produtos />}/>
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;