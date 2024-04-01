import axios from 'axios';

class ProfileService {

    async getProfileDetails(id: String, feedId: String): Promise<ProfileDetails> {
        try {
            const response = await axios.get(import.meta.env.VITE_API_URL + '/persons/' + id + '/profiles/' + feedId);
            console.log('Feed details:', response.data);
            return response.data;
        } catch (error) {
            console.error('Error fetching persons:', error);
            throw error;
        }
    }

    async addNewProfile(id: String,
        data: ProfileDetails): Promise<void> {

        const dataToSend =
        {
            title: data.title,
            description: data.description,
            skills: data.skills
        }

        try {
            await axios.post(import.meta.env.VITE_API_URL + '/persons/' + id + '/profiles/', dataToSend);
        } catch (error) {
            console.error('Error fetching persons:', error);
            throw error;
        }
    }

    async updateProfileDetails(id: String,
        data: ProfileDetails): Promise<void> {

        const dataToSend =
        {
            title: data.title,
            description: data.description,
            skills: data.skills
        }

        try {
            await axios.put(import.meta.env.VITE_API_URL + '/persons/' + id + '/profiles/' + data.id, dataToSend);
        } catch (error) {
            console.error('Error fetching persons:', error);
            throw error;
        }
    }
}

export interface ProfileDetails {
    id: String,
    title: String,
    description: number,
    skills: String,
    createdUtc: String
}

export default new ProfileService();
