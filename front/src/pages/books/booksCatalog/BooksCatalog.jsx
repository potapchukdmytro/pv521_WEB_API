import { useState, useEffect } from "react";
import "./BooksCatalog.css";
import { env } from "../../../env";
import axios from "axios";
import { Link } from "react-router";

const BooksCatalog = () => {
    const [page, setPage] = useState(1);
    const [sortBy, setSortBy] = useState("created");
    const [payload, setPayload] = useState({
        page: 1,
        pageSize: 40,
        total: 1,
        pageCount: 1,
        items: [],
    });

    const fetchBooks = async (pageSize = 40) => {
        const url = `${env.apiUrl}/books?page=${page}&pageSize=${pageSize}&sortBy=${sortBy}`;
        try {
            const response = await axios.get(url);
            const { data } = response;
            setPayload(data.payload);
        } catch (error) {
            console.log(error);
        }
    };

    const changeSortValue = (event) => {
        const select = event.target;
        setSortBy(select.value);
    };

    useEffect(() => {
        fetchBooks();
    }, [page, sortBy]);

    return (
        <main className="catalog">
            <div className="catalog-container">
                <section className="catalog-header">
                    <div>
                        <span className="catalog-label">Бібліотека</span>

                        <h1>Каталог книг</h1>

                        <p>
                            Знайдіть книгу, яка стане вашою наступною улюбленою
                            історією.
                        </p>
                    </div>

                    <div className="catalog-search">
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

                        <input
                            type="text"
                            placeholder="Пошук за назвою або автором..."
                        />
                    </div>
                </section>

                <section className="catalog-toolbar">
                    <div className="catalog-categories">
                        <button className="category active">Усі книги</button>
                        <button className="category">Художні</button>
                        <button className="category">Фантастика</button>
                        <button className="category">Детективи</button>
                        <button className="category">Бізнес</button>
                        <button className="category">IT</button>
                    </div>

                    <select
                        className="catalog-sort"
                        value={sortBy}
                        onChange={changeSortValue}
                    >
                        <option value="created">Нові книги</option>
                        <option value="priceasc">Ціна: від дешевих</option>
                        <option value="pricedesc">Ціна: від дорогих</option>
                        <option value="rating">За рейтингом</option>
                    </select>
                </section>

                <div className="catalog-info">
                    <span>
                        Знайдено <strong>{payload.total} книг</strong>
                    </span>
                </div>

                <section className="books-grid">
                    {payload.items.map((book) => (
                        <article className="book-card" key={book.id}>
                            <div className="book-image-container">
                                <img
                                    className="book-image"
                                    src={`${env.imagesUrl}/books/${book.image}`}
                                    alt={book.title}
                                />

                                <button className="favorite-button">
                                    <svg
                                        width="20"
                                        height="20"
                                        viewBox="0 0 24 24"
                                        fill="none"
                                        stroke="currentColor"
                                        strokeWidth="2"
                                    >
                                        <path d="M20.8 4.6a5.5 5.5 0 0 0-7.8 0L12 5.6l-1-1a5.5 5.5 0 0 0-7.8 7.8l1 1L12 21l7.8-7.6 1-1a5.5 5.5 0 0 0 0-7.8z" />
                                    </svg>
                                </button>
                            </div>

                            <div className="book-content">
                                <div className="book-rating">
                                    <span>★</span>
                                    {book.rating}
                                </div>

                                <h2>{book.title}</h2>

                                <p>
                                    {book.author
                                        ? book.author.name
                                        : "Невідомий"}
                                </p>

                                <div className="book-footer">
                                    <strong>{book.price} ₴</strong>

                                    <Link
                                        to={`/book/${book.id}`}
                                        className="book-details"
                                    >
                                        Детальніше
                                    </Link>
                                </div>
                            </div>
                        </article>
                    ))}
                </section>

                <Pagination
                    currentPage={page}
                    onPageChange={setPage}
                    totalPages={payload.pageCount}
                />
            </div>
        </main>
    );
};

function Pagination({ currentPage, totalPages, onPageChange }) {
    const getPages = () => {
        const pages = [];

        if (totalPages <= 7) {
            for (let i = 1; i <= totalPages; i++) {
                pages.push(i);
            }

            return pages;
        }

        pages.push(1);

        if (currentPage > 4) {
            pages.push("...");
        }

        const start = Math.max(2, currentPage - 1);
        const end = Math.min(totalPages - 1, currentPage + 1);

        for (let i = start; i <= end; i++) {
            pages.push(i);
        }

        if (currentPage < totalPages - 3) {
            pages.push("...");
        }

        pages.push(totalPages);

        return pages;
    };

    const pages = getPages();

    return (
        <div className="pagination">
            <button
                className="pagination-arrow"
                disabled={currentPage === 1}
                onClick={() => onPageChange(currentPage - 1)}
            >
                ←
            </button>

            {pages.map((page, index) => {
                if (page === "...") {
                    return (
                        <span key={`dots-${index}`} className="pagination-dots">
                            ...
                        </span>
                    );
                }

                return (
                    <button
                        key={page}
                        className={`pagination-page ${
                            currentPage === page ? "active" : ""
                        }`}
                        onClick={() => onPageChange(page)}
                    >
                        {page}
                    </button>
                );
            })}

            <button
                className="pagination-arrow"
                disabled={currentPage === totalPages}
                onClick={() => onPageChange(currentPage + 1)}
            >
                →
            </button>
        </div>
    );
}

export default BooksCatalog;
