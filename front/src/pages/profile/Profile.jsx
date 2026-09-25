import { useEffect, useState } from "react";
import "./Profile.css";
import { env } from "../../env";
import Loader from "../../components/loader/Loader";
import { api } from "../../api";

export default function Profile() {
    const [isEditing, setIsEditing] = useState(false);
    const [userData, setUserData] = useState(null);

    const [profile, setProfile] = useState({
        name: "Dmytro Potapchuk",
        email: "dmytro@gmail.com",
        about: "Люблю читати фантастику, технічну літературу та книги про програмування.",
    });

    const [form, setForm] = useState(profile);

    const handleSave = () => {
        setProfile(form);
        setIsEditing(false);
    };

    const handleCancel = () => {
        setForm(profile);
        setIsEditing(false);
    };

    const fetchUserData = async () => {
        const response = await api.get("auth/me");
        const { data } = response;
        console.log(response);
        
        setUserData(data.payload);
    };

    useEffect(() => {
        fetchUserData();
    }, []);

    if(!userData) {
        return <Loader/>
    }

    return (
        <main className="profile-page">
            <div className="profile-container">
                <section className="profile-card">
                    <div className="profile-header">
                        <div className="profile-user">
                            <div className="profile-avatar">
                                <span>DP</span>
                            </div>

                            <div>
                                <h1>{userData.firstName + " " + userData.lastName}</h1>
                                <p>{userData.email}</p>
                            </div>
                        </div>

                        {!isEditing ? (
                            <button
                                className="profile-edit-button"
                                onClick={() => setIsEditing(true)}
                            >
                                Редагувати
                            </button>
                        ) : (
                            <div className="profile-edit-actions">
                                <button
                                    className="profile-cancel-button"
                                    onClick={handleCancel}
                                >
                                    Скасувати
                                </button>

                                <button
                                    className="profile-save-button"
                                    onClick={handleSave}
                                >
                                    Зберегти
                                </button>
                            </div>
                        )}
                    </div>

                    <div className="profile-divider" />

                    <div className="profile-content">
                        <div className="profile-main">
                            <div className="profile-section">
                                <div className="profile-section-header">
                                    <div>
                                        <h2>Особиста інформація</h2>
                                        <p>Основні дані вашого профілю</p>
                                    </div>
                                </div>

                                <div className="profile-fields">
                                    <div className="profile-field">
                                        <label>Ім'я</label>

                                        {isEditing ? (
                                            <input
                                                value={form.name}
                                                onChange={(e) =>
                                                    setForm({
                                                        ...form,
                                                        name: e.target.value,
                                                    })
                                                }
                                            />
                                        ) : (
                                            <div className="profile-field-value">
                                                {userData.userName}
                                            </div>
                                        )}
                                    </div>

                                    <div className="profile-field">
                                        <label>Email</label>

                                        {isEditing ? (
                                            <input
                                                type="email"
                                                value={form.email}
                                                onChange={(e) =>
                                                    setForm({
                                                        ...form,
                                                        email: e.target.value,
                                                    })
                                                }
                                            />
                                        ) : (
                                            <div className="profile-field-value">
                                                {userData.email}
                                            </div>
                                        )}
                                    </div>

                                    <div className="profile-field profile-field-full">
                                        <label>Про себе</label>

                                        {isEditing ? (
                                            <textarea
                                                rows="5"
                                                value={form.about}
                                                onChange={(e) =>
                                                    setForm({
                                                        ...form,
                                                        about: e.target.value,
                                                    })
                                                }
                                            />
                                        ) : (
                                            <p className="profile-about">
                                                {userData.firstName + " " + userData.lastName}
                                            </p>
                                        )}
                                    </div>
                                </div>
                            </div>
                        </div>

                        <aside className="profile-sidebar">
                            <a href="/favorites" className="profile-menu-item">
                                <div className="profile-menu-icon">
                                    <svg
                                        viewBox="0 0 24 24"
                                        fill="none"
                                        stroke="currentColor"
                                        strokeWidth="2"
                                    >
                                        <path d="M20.8 4.6a5.5 5.5 0 0 0-7.8 0L12 5.6l-1-1a5.5 5.5 0 0 0-7.8 7.8l1 1L12 21l7.8-7.6 1-1a5.5 5.5 0 0 0 0-7.8z" />
                                    </svg>
                                </div>

                                <div>
                                    <strong>Улюблені</strong>
                                    <span>Збережені книги</span>
                                </div>
                            </a>

                            <a href="/viewed" className="profile-menu-item">
                                <div className="profile-menu-icon">
                                    <svg
                                        viewBox="0 0 24 24"
                                        fill="none"
                                        stroke="currentColor"
                                        strokeWidth="2"
                                    >
                                        <circle cx="12" cy="12" r="9" />
                                        <path d="M12 7v5l3 2" />
                                    </svg>
                                </div>

                                <div>
                                    <strong>Переглянуті</strong>
                                    <span>Історія переглядів</span>
                                </div>
                            </a>

                            <a href="/settings" className="profile-menu-item">
                                <div className="profile-menu-icon">
                                    <svg
                                        viewBox="0 0 24 24"
                                        fill="none"
                                        stroke="currentColor"
                                        strokeWidth="2"
                                    >
                                        <circle cx="12" cy="12" r="3" />
                                        <path d="M19.4 15a1.7 1.7 0 0 0 .3 1.9l.1.1-2.8 2.8-.1-.1a1.7 1.7 0 0 0-1.9-.3 1.7 1.7 0 0 0-1 1.6v.2h-4V21a1.7 1.7 0 0 0-1-1.6 1.7 1.7 0 0 0-1.9.3l-.1.1L4.2 17l.1-.1a1.7 1.7 0 0 0 .3-1.9A1.7 1.7 0 0 0 3 14H2.8v-4H3a1.7 1.7 0 0 0 1.6-1 1.7 1.7 0 0 0-.3-1.9L4.2 7 7 4.2l.1.1a1.7 1.7 0 0 0 1.9.3 1.7 1.7 0 0 0 1-1.6v-.2h4V3a1.7 1.7 0 0 0 1 1.6 1.7 1.7 0 0 0 1.9-.3l.1-.1L19.8 7l-.1.1a1.7 1.7 0 0 0-.3 1.9 1.7 1.7 0 0 0 1.6 1h.2v4H21a1.7 1.7 0 0 0-1.6 1Z" />
                                    </svg>
                                </div>

                                <div>
                                    <strong>Налаштування</strong>
                                    <span>Акаунт і безпека</span>
                                </div>
                            </a>
                        </aside>
                    </div>
                </section>
            </div>
        </main>
    );
}
