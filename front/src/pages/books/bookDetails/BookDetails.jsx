import { Link, useParams } from "react-router";
import "./BookDetails.css";
import { useEffect, useState } from "react";
import { env } from "../../../env";
import axios from "axios";
import Loader from "../../../components/loader/Loader";

export default function BookDetails() {
    const [book, setBook] = useState(null);

    const { id } = useParams();

    const fetchBook = async () => {
        const url = `${env.apiUrl}/books/${id}`;
        try {
            const response = await axios.get(url);
            const { data } = response;
            setBook(data.payload);
        } catch (error) {
            console.log(error);
        }
    };

    useEffect(() => {
        fetchBook();
    }, []);

    if (!book) {
        return <Loader />;
    }

    return (
        <main className="book-details-page">
            <div className="book-details-container">
                <Link to="/" className="back-link">
                    ← Назад до каталогу
                </Link>

                <section className="book-details-main">
                    <div className="book-cover-section">
                        <div className="book-cover-wrapper">
                            <img
                                src={`${env.imagesUrl}/books/${book.image}`}
                                alt={book.title}
                                className="book-cover"
                            />
                        </div>

                        <button className="favorite-details-button">
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
                            Додати в улюблені
                        </button>
                    </div>

                    <div className="book-info">
                        <span className="book-category">Фентезі</span>

                        <h1>{book.title}</h1>

                        <a href="/authors/1" className="book-author">
                            {book.author ? book.author.name : "Невідомий"}
                        </a>

                        <div className="book-rating-row">
                            <div className="book-rating-large">
                                <span>★</span>
                                <strong>{book.rating}</strong>
                            </div>

                            <span className="reviews-count">{book.reviews} відгуків</span>
                        </div>

                        <p className="book-description">{book.description}</p>

                        <div className="book-meta-grid">
                            <div className="book-meta-item">
                                <span>Рік видання</span>
                                <strong>{book.year}</strong>
                            </div>

                            <div className="book-meta-item">
                                <span>Кількість сторінок</span>
                                <strong>{book.pages}</strong>
                            </div>

                            <div className="book-meta-item">
                                <span>Мова</span>
                                <strong>{book.language}</strong>
                            </div>

                            <div className="book-meta-item">
                                <span>Видавництво</span>
                                <strong>{book.publisher}</strong>
                            </div>
                        </div>

                        <div className="book-buy-card">
                            <div>
                                <span className="price-label">Ціна</span>
                                <div className="book-price">{book.price} ₴</div>
                            </div>

                            <button className="buy-button">
                                Додати в кошик
                            </button>
                        </div>
                    </div>
                </section>

                <section className="book-additional-section">
                    <div className="book-additional-card">
                        <h2>Про книгу</h2>

                        <p>{book.description}</p>
                    </div>

                    <div className="book-characteristics-card">
                        <h2>Характеристики</h2>

                        <div className="characteristic-row">
                            <span>Автор</span>
                            <strong>
                                {book.author ? book.author.name : "Невідомий"}
                            </strong>
                        </div>

                        <div className="characteristic-row">
                            <span>Жанр</span>
                            <strong>Фентезі</strong>
                        </div>

                        <div className="characteristic-row">
                            <span>Рік</span>
                            <strong>{book.year}</strong>
                        </div>

                        <div className="characteristic-row">
                            <span>Сторінок</span>
                            <strong>{book.pages}</strong>
                        </div>

                        <div className="characteristic-row">
                            <span>Мова</span>
                            <strong>{book.language}</strong>
                        </div>

                        <div className="characteristic-row">
                            <span>Видавництво</span>
                            <strong>{book.publisher}</strong>
                        </div>

                        <div className="characteristic-row">
                            <span>ISBN</span>
                            <strong>{book.isbn}</strong>
                        </div>
                    </div>
                </section>
            </div>
        </main>
    );
}
