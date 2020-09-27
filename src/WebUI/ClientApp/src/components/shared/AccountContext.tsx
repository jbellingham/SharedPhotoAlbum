import { Meteor } from 'meteor/meteor'
import React from 'react'
import { withTracker } from 'meteor/react-meteor-data'

export interface IAccountProps {
    account: {
        user: Meteor.User | null
        userId: string | null
        isLoggedIn: boolean
    }
}

const AccountContext = React.createContext('account')

function Provider(props: any) {
    return <AccountContext.Provider value={props.account}>{props.children}</AccountContext.Provider>
}

export const withAccount = withTracker(
    (props: any): IAccountProps => {
        const user = Meteor.isServer ? null : Meteor.user()
        const userId = Meteor.isServer ? null : Meteor.userId()
        return {
            account: {
                user,
                userId,
                isLoggedIn: !!userId,
            },
        }
    },
)

export const AccountProvider = withAccount(Provider)
export const AccountConsumer = AccountContext.Consumer
