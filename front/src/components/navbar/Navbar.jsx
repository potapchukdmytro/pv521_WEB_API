import "./Navbar.css";

const Navbar = () => {
    return (
        <header className="navbar">
            <div className="navbar-container">
                <a href="/" className="navbar-logo">
                    <span className="navbar-logo-icon">B</span>
                    <span>Bookly</span>
                </a>

                <nav className="navbar-links">
                    <a href="/" className="navbar-link">
                        Головна
                    </a>

                    <a href="/catalog" className="navbar-link active">
                        Каталог
                    </a>

                    <a href="/authors" className="navbar-link">
                        Автори
                    </a>

                    <a href="/favorites" className="navbar-link">
                        Улюблені
                    </a>
                </nav>

                <div className="navbar-actions">
                    <button className="navbar-search-button" aria-label="Пошук">
                        <svg
                            width="20"
                            height="20"
                            viewBox="0 0 24 24"
                            fill="none"
                            stroke="currentColor"
                            strokeWidth="2"
                        >
                            <circle cx="11" cy="11" r="8" />
                            <path d="m21 21-4.3-4.3" />
                        </svg>
                    </button>

                    <button className="navbar-profile">
                        <div className="navbar-avatar">
                            <svg
                                width="20"
                                height="20"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                strokeWidth="2"
                            >
                                <path d="M20 21a8 8 0 0 0-16 0" />
                                <circle cx="12" cy="7" r="4" />
                            </svg>
                        </div>

                        <span>Профіль</span>
                    </button>
                </div>
            </div>
        </header>
    );
};

export default Navbar;
