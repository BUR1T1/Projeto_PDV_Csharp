// src/components/Header.jsx
import React from 'react';
import { Link } from 'react-router-dom';
import './Header.css';

function Header() {
  return (
    <header className="header-container">
      <nav className="navbar">
        <Link to="/" className="brand-logo">WebPDV</Link>
        <ul className="nav-links">
          <li>
            <Link to="/cadastro-cliente">Cadastro Cliente</Link>
          </li>
          <li>
            <Link to="/cadastro-vendedor">Cadastro Vendedor</Link>
          </li>
          <li>
            <Link to="/vendas-loja">Vendas da Loja</Link>
          </li>
          <li>
            <Link to="/venda-page">Venda Page</Link> 
          </li>
          <li>
            <Link to="/Produtos">Produtos</Link> 
          </li>
        </ul>
      </nav>
    </header>
  );
}

export default Header;