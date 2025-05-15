import {
    HOME_ROUTE,
    ARTICLES_ROUTE,
    LOGIN_ROUTE,
    REGISTER_ROUTE,
    REGISTER_CONFIRM_ROUTE,
    FORUM_ROUTE
} from './utils/constants';

import {
    ArticlesPage,
    Login,
    Register,
    ForumPage,
    RegisterConfirm,
    Home
} from './pages';

import QuestionPage from './pages/Forum/QuestionPage';

export const publishRoutes = [
    {
        path: HOME_ROUTE,
        Component: Home
    },
    {
        path: ARTICLES_ROUTE,
        Component: ArticlesPage
    },
    {
        path: LOGIN_ROUTE,
        Component: Login
    },
    {
        path: REGISTER_ROUTE,
        Component: Register
    },
    {
        path: REGISTER_CONFIRM_ROUTE,
        Component: RegisterConfirm
    },
    {
        path: FORUM_ROUTE,
        Component: ForumPage
    },
    {
        path: "/questions/:id",
        Component: QuestionPage
    }
];