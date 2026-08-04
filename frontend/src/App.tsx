import {BrowserRouter, Routes, Route} from 'react-router-dom'
import HomePage from './pages/HomePage'
import MovieCreatePage from './pages/MovieCreatePage'
import MovieDetailPage from './pages/MovieDetailPage'
import TvShowDetailPage from './pages/TvShowDetailPage'
import TvShowCreatePage from "./pages/TvShowCreatePage.tsx";

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<HomePage/>}/>
                <Route path="/movies/new" element={<MovieCreatePage/>}/>
                <Route path="/movies/:id" element={<MovieDetailPage/>}/>
                <Route path="/tvshows/new" element={<TvShowCreatePage/>}/>
                <Route path="/tvshows/:id" element={<TvShowDetailPage/>}/>
            </Routes>
        </BrowserRouter>
    )
}