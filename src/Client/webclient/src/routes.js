import {
    HOME_ROUTE,
    ARTICLES_ROUTE,
    LOGIN_ROUTE,
    REGISTER_ROUTE,
    REGISTER_CONFIRM_ROUTE,
    FORUM_ROUTE,
    PROFILE_ROUTE
} from './utils/constants';

import {
    ArticlesPage,
    ArticleEditor,
    ArticlePage,
    Login,
    Register,
    ForumPage,
    RegisterConfirm,
    Home,
    ProfilePage,
    AdminApprovalPage
} from './pages';

import QuestionPage from './pages/Forum/QuestionPage';
import { Component } from 'react';

export const publishRoutes = [
    {
        path: HOME_ROUTE,
        Component: ArticlesPage
    },
    {
        path: ARTICLES_ROUTE,
        Component: ArticlesPage
    },
    {
        path: "articles/:id",
        Component: ArticlePage
    },
    {
        path: "articles/create",
        Component: ArticleEditor
    },
    {
        path: "articles/create/:id",
        Component: ArticleEditor
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
    },
    {
        path: PROFILE_ROUTE,
        Component: ProfilePage
    },
    {
        path: "/dashboard",
        Component: AdminApprovalPage
    }
];