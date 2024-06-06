import { routeNames } from "../../../routesConstants"

export interface NavItem {
    name: string,
    to: string
}

export const guestNavItem: NavItem[] = [
    {
        name: 'Home',
        to: routeNames.index,
    }
]

export const userNavItem: NavItem[] = [
    {
        name: 'Home',
        to: routeNames.index,
    },
    {
        name: 'Borrowing',
        to: routeNames.index,
    }
]

export const adminNavItem: NavItem[] = [
    {
        name: 'Home',
        to: routeNames.index,
    },
    {
        name: 'Book',
        to: routeNames.bookList,
    },
    {
        name: "Category",
        to: routeNames.categoryList
    }
]