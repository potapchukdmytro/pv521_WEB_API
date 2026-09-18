import "./App.css";
import Navbar from "./components/navbar/Navbar";
import BooksCatalog from "./pages/books/booksCatalog/BooksCatalog";

function App() {
    return (
        <div>
            <Navbar />
            <BooksCatalog />
        </div>
    );
}

export default App;
