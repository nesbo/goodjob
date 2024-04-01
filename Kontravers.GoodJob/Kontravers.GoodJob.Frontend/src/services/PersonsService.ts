import axios from 'axios';

class PersonsService {
    async getPersons(): Promise<Person[]> {
        try {
            const response = await axios.get(import.meta.env.VITE_API_URL + '/persons');
            console.log('Persons:', response.data);
            return response.data.items;
        } catch (error) {
            console.error('Error fetching persons:', error);
            throw error;
        }
    }
    async getPersonDetails(id: String): Promise<PersonDetails> {
        try {
            const response = await axios.get(import.meta.env.VITE_API_URL + '/persons/' + id);
            console.log('Person Details:', response.data);
            return response.data;
        } catch (error) {
            console.error('Error fetching persons:', error);
            throw error;
        }
    }
}

export interface Person {
    id: String;
    name: String;
    // Add more properties as needed
}

export interface PersonDetails {
    id: String;
    name: String;
    email: String;
    organisationId: String;
    upworkRssFeeds: UpworkRssFeed[];
    profiles: Profile[];
    // Add more properties as needed
}

export interface UpworkRssFeed {
    id: String;
    title: String;
    // Add more properties as needed
}

export interface Profile {
    id: number;
    title: String;
    // Add more properties as needed
}

export default new PersonsService();
