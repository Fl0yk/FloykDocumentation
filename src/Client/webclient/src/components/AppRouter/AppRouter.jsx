import { Routes, Route } from 'react-router-dom'
import { publishRoutes } from '../../routes'
import NotFoundPage from './NotFoundPage'

const AppRouter = () => {
    return (
        <Routes>
            {publishRoutes.map(({path, Component}) => (
                <Route key={path} path={path} element={<Component />} />
            ))}
            <Route key='*' element={<NotFoundPage />} />
        </Routes>
    )
}

export default AppRouter;