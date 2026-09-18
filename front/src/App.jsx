import { Route, Routes } from "react-router";
import "./App.css";
import Navbar from "./components/navbar/Navbar";
import BookDetails from "./pages/books/bookDetails/BookDetails";
import BooksCatalog from "./pages/books/booksCatalog/BooksCatalog";

function App() {
    return (
        <div>
            <Navbar />
            <Routes>
                <Route path="/" element={<BooksCatalog />} />
                <Route path="/book/:id" element={<BookDetails />} />
            </Routes>
        </div>
    );
}

export default App;
