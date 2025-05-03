import {
    HOME_ROUTE,
    ARTICLES_ROUTE,
    AUTH_ROUTE,
    FORUM_ROUTE
} from './utils/constants';

import {
    Articles,
    Auth,
    Forum,
    Home
} from './pages';

import Question from './pages/Forum/QuestionPage';

export const publishRoutes = [
    {
        path: HOME_ROUTE,
        Component: Home
    },
    {
        path: ARTICLES_ROUTE,
        Component: Articles
    },
    {
        path: AUTH_ROUTE,
        Component: Auth
    },
    {
        path: FORUM_ROUTE,
        Component: Forum
    },
    {
        path: "/question/:id",
        Component: Question
    }
];